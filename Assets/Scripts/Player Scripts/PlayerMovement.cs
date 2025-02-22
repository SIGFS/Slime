
//	Inspired by @DawnosaurDev at youtube.com/c/DawnosaurStudios

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Animator playerAnim;
	public PlayerData Data;

	#region Variables
	//Components
	public Rigidbody2D RB { get; private set; }

	//Action Controls
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }

	//Timers
	public float LastOnGroundTime { get; private set; }
	private bool isMoving = false;

	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;

	private Vector2 _moveInput;
	public float LastPressedJumpTime { get; private set; }

	//Set up in Inspector
	[Header("Checks")]
	[SerializeField] private Transform _groundCheckPoint;
	[SerializeField] public Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(6)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
    #endregion

    #region Unity Methods
    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		playerAnim = GetComponent<Animator>();
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
		#region TIMERS
		LastOnGroundTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		#endregion

		#region INPUT HANDLER
		if (!GameManager.instance.isPaused)
		{
			_moveInput.x = Input.GetAxisRaw("Horizontal");
			_moveInput.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
			_moveInput.x = 0f;
			_moveInput.y = 0f;
		}


		if (_moveInput.x != 0)
			if(isMoving == false)
            {
				isMoving = true;
				StartCoroutine(PlayMoveSound());	
			}
			CheckDirectionToFace(_moveInput.x > 0);

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J) && !GameManager.instance.isPaused)
		{
			OnJumpInput();
		}

		
		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J) && !GameManager.instance.isPaused)
		{
			OnJumpUpInput();
		}
		
		#endregion

		#region COLLISION CHECKS
		if (!IsJumping)
		{
			//Ground Check

			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{
				LastOnGroundTime = Data.coteTime; //if so sets the lastGrounded to coyoteTime
				playerAnim.SetBool("Jumping", false);
			}
		}
		#endregion

		#region JUMP CHECKS
		if (IsJumping && RB.velocity.y <= 0)
		{
			IsJumping = false;
			_isJumpFalling = true;
		}

		if (LastOnGroundTime > 0 && !IsJumping)
		{
			_isJumpCut = false;

			if (!IsJumping)
            {
				_isJumpFalling = false;
			}
				
		}

		//Jump
		if (CanJump() && LastPressedJumpTime > 0)
		{
			IsJumping = true;
			_isJumpCut = false;
			_isJumpFalling = false;
			Jump();
		}
		#endregion

		#region GRAVITY
		if (RB.velocity.y < 0 && _moveInput.y < 0)
		{
			//Much higher gravity if holding down
			SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
		}
		else if (_isJumpCut)
		{
			//Higher gravity if jump button released
			SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
		}
		else if (RB.velocity.y < 0)
		{
			//Higher gravity if falling
			SetGravityScale(Data.gravityScale * Data.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			SetGravityScale(Data.gravityScale);
		}
		#endregion
	}

	private void FixedUpdate()
	{
		Run(1);
	}
    #endregion

    #region Input Callbacks
    //Methods which whandle input detected in Update()
    public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut())
			_isJumpCut = true;
	}
	#endregion

	#region General Methods
	public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}
	#endregion

	#region Run Methods
	private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//We can reduce our control using Lerp() this smooths changes to our direction and speed
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve our current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis to apply to the player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
	#endregion

	#region Jump Methods
	private void Jump()
	{
		AudioManager.Instance.playSlimeJumpLand();
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = Data.jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;
		playerAnim.SetBool("Jumping", true);
		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

	public void BouncePad(float bounceForce)
    {
		AudioManager.Instance.playSlimeBounce();
		playerAnim.SetBool("Jumping", true);
		RB.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
	}

	public void BounceEnemy(float bounceForce)
    {
		AudioManager.Instance.playSlimeBounce();
		playerAnim.SetBool("Jumping", true);
		RB.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
	}

	#endregion

	#region Check Methods
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		bool canJump = LastOnGroundTime > 0 && !IsJumping;
		return canJump;
	}

	private bool CanJumpCut()
	{
		return IsJumping && RB.velocity.y > 0;
	}
	#endregion

	#region Editor Methods
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
	}
	#endregion

	IEnumerator PlayMoveSound()
    {
		AudioManager.Instance.playMovement();
		yield return new WaitForSeconds(0.4f);
		isMoving = false;
	}
}