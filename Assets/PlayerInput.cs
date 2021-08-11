using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    ArrayList InputsList;
    
    public int frame;
    public Inputs currInput;
    public int lastMovementInput = 0;
    //               1          2                 3                4             5               6             7               8             9
    public bool DBACK = false, DOWN = false, DFORWARD = false, BACK = false, NEUTRAL = false, FORWARD = false, UBACK = false, UP = false, UFORWARD = false;
    int currMovementKey = 0;
    int currInputKey = 0;
    //dash input recording is to record the LAST BUTTON PRESSED BEFORE RETURNING TO NETURAL
    //for example: I pressed forward, released it, so now dashInputRecording is 6 because 6 is forward and I returned to neutral
    //so if I were to press 6 again after releasing it, I would dash
    public int dashInputRecording = 0;
    public int dashTimer;
    //hold D timer, if hold D is held down for the below number's amount of frames, send a hold D, if its released set the timer to 0
    //there is also a semi-hold D timer for people that have that, but that only will be sent if we're above the semi but below the full
    int semiHoldDTimer = 0; //max is 15
    int fullHoldDTimer = 0; //max is 30

    //only for rotating the char
    Quaternion deltaRot;

    public int recoveryFrames = 0;
    public bool comboWindow;


    //use attacks
    public int accemptableInputs = 0;
    //movement
    public bool isDashing;
    //hands 2 show who's who
    public GameObject bobrighthand;
    public GameObject boblefthand;
    public GameObject joerighthand;
    public GameObject joelefthand;
    public bool isGrounded = true;
    public bool down = false;
    public int hurtTime = 0;
    //block checker
    public bool isBlocking = false;
    public bool busy = false;
    //begin quarter circle recordings
    int QCFTimer = 0;
    public bool[] QCF = new bool[3];
    //end quarter circle recordings
    //begin backwards DP recordings(Z-input)
    int BDPTimer = 0;
    public bool[] BDP = new bool[3];    
    //end backwards DP recordings(Z-input)
    //begin half circle forward recordings
    int HCFTimer = 0;
    public bool[] HCF = new bool[5];
    //end half circle forward recordings
    //begin quarter circle backward recordings
    int QCBTimer = 0;
    public bool[] QCB = new bool[3];
    //end quarter circle backward recordings
    //begin Forwards DP recordings(Z-input)
    int FDPTimer = 0;
    public bool[] FDP = new bool[3];
    //end forwards DP recordings(Z-input)
    //begin half circle backward recordings
    int HCBTimer = 0;
    public bool[] HCB = new bool[5];
    //end half circle backward recordings
    [SerializeField, SerializeReference]
    public Character currChar;
    
    public Rigidbody model;
    public GameObject hurtbox;
    //key = ASDF 
    //movement = arrow keys
    //frame = the frame that these inputs were inputted on
    public struct Inputs
    {
        public Inputs(int key, int movement, int frame){
            KEY = key;
            MOVEMENT = movement;
            FRAME = frame;
            
    }
        public int KEY { get; }
        public int MOVEMENT { get; }
        public int FRAME { get; }
        
       
    }
    public void nameHurtbox(string name)
    {
       HurtBox hscript = hurtbox.GetComponent<HurtBox>();
        hscript.moveName = name;
    }
    public void damageHurtbox(int damage)
    {
        HurtBox hscript = hurtbox.GetComponent<HurtBox>();
        hscript.damage = damage;
    }
    public void resetholdDTimer()
    {
        fullHoldDTimer = 0;
    }
    public void resetanim()
    {
        currChar.resetAnim(this);
    }
    public void forceIdle()
    {
        currChar.isDashing = false;
        currChar.Idle(this);
        isDashing = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        joelefthand.SetActive(false);
        joerighthand.SetActive(false);
        InputsList = new ArrayList();
        frame = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            if (!isGrounded)
            {
                isGrounded = true;
                currChar.Idle(this);
                Debug.Log("landed");
                fullHoldDTimer = 0;
                semiHoldDTimer = 0;
            }
            else
            {
                return;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (down)
        {
         return;
        }
        if (Input.GetButton("BobSwap"))
        {
            Debug.Log("Bob Swap");
            joelefthand.SetActive(false);
            joerighthand.SetActive(false);
            bobrighthand.SetActive(true);
            boblefthand.SetActive(true);
            currChar.swap(this);
            if(currChar.name == "Joe")
            {
                currChar.swap(this);
            }
            return;
        }
        if (Input.GetButton("JoeSwap"))
        {
            Debug.Log("Joe Swap");
            joelefthand.SetActive(true);
            joerighthand.SetActive(true);
            boblefthand.SetActive(false);
            bobrighthand.SetActive(false);
            currChar.swap(this);
            if(currChar.name == "Bob")
            {
                currChar.swap(this);
            }
            
            return;
        }
            //Debug.Log(fullHoldDTimer);
            Inputs currFrame = new Inputs(inputProcessor(), movementProcessor(), frame);
        InputsList.Add(currFrame);
        currInput = (Inputs)InputsList[InputsList.Count-1];
        currMovementKey = currInput.MOVEMENT;
        currInputKey = currInput.KEY;
        if (currInput.KEY > 0)
        {
           /* if (QCF(currInput))
            {
                Debug.Log("Quarter Circle Forward!");
            }*/
        }
        if (currInput.KEY > 0 || currInput.MOVEMENT > 0)
        {
         // Debug.Log(currInput.KEY + " " + currInput.MOVEMENT + " " + frame);
        }
        //begin input interp
        //normal movement input

        switch(currMovementKey){
            
            case 1:
                currChar.direction = 1;
                
               // Debug.Log("DBACK");
                deltaRot = Quaternion.Euler(0, 135, 0);
                model.MoveRotation(deltaRot);
                if(fullHoldDTimer > 0)
                {
                    break;
                }
                if(!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                
                if (!isBlocking && ((dashInputRecording == 1 && dashTimer != 0)|| isDashing) && recoveryFrames == 0)
                {
                    
                    //Debug.Log("DBACK DASH");
                    dashTimer = 2;
                    currChar.Dasher(this);
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 1;
                    isDashing = false;
                }
                break;
            case 2:
                currChar.direction = 2;
                
               // Debug.Log("DOWN");
                deltaRot = Quaternion.Euler(0, 90, 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 2 && dashTimer != 0 )|| isDashing) && recoveryFrames == 0)
                {
                    //Debug.Log("DOWN DASH");
                    dashTimer = 2;
                    currChar.Dasher(this);
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 2;
                    isDashing = false;
                }
                break;
            case 3:
                currChar.direction = 3;
                
               // Debug.Log("DFORWARD");
                deltaRot = Quaternion.Euler(0, 45, 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 3 && dashTimer != 0)|| isDashing) && recoveryFrames == 0)
                {
                    //Debug.Log("DFORWARD DASH");
                    dashTimer = 2;
                    currChar.Dasher(this);
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 3;
                    isDashing = false;
                }
                break;
            case 4:
                currChar.direction = 4;
                
               // Debug.Log("BACK");
                deltaRot = Quaternion.Euler(0, 180, 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 4 && dashTimer != 0) || isDashing) && recoveryFrames == 0)
                {
                    currChar.Dasher(this);
                    //Debug.Log("BACK DASH");
                    dashTimer = 2;
                    isDashing = true;

                }
                else
                {
                    lastMovementInput = 4;
                    isDashing = false;
                }
                break; 
            case 5:
                //Debug.Log("NEUTRAL");
                if(dashInputRecording == lastMovementInput)
                {
                    dashTimer = 0;
                    dashInputRecording = 5;
                    if (isGrounded)
                    {
                        currChar.isDashing = false;
                        currChar.Idle(this);
                        isDashing = false;
                        
                    }
                    }
                    else
                {
                    dashInputRecording = lastMovementInput;
                    isDashing = false;
                }
                dashTimer = 30;
                break;
            case 6:
                
                currChar.direction = 6;
                
                deltaRot = Quaternion.Euler(0, 0, 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                    //Debug.Log("FORWARD");
                }
                if (!isBlocking && ((dashInputRecording == 6 && dashTimer != 0)|| isDashing) && recoveryFrames == 0)
                {
                    //.Log("FORWARD DASH");
                   
                    currChar.Dasher(this);
                    isDashing = true;
                    dashTimer = 2;

                    
                }
                else
                {
                    lastMovementInput = 6;
                    isDashing = false;
                }
                break;
            case 7:
                currChar.direction = 7;
                
               // Debug.Log("UBACK");
                deltaRot = Quaternion.Euler(0, 225, 0);
                model.MoveRotation(deltaRot);
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 7 && dashTimer != 0) || isDashing) && recoveryFrames == 0)
                {
                   // Debug.Log("UBACK DASH");
                    currChar.Dasher(this);
                    dashTimer = 2;
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 7;
                    isDashing = false;
                }
                break;
            case 8:
                currChar.direction = 8;
               // Debug.Log("UP");
                
                deltaRot =  Quaternion.Euler(0, 270, 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 8 && dashTimer != 0)|| isDashing) && recoveryFrames == 0)
                {
                   // Debug.Log("UP DASH");
                    currChar.Dasher(this);
                    dashTimer = 2;
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 8;
                    isDashing = false;
                }
                break;
            case 9:
                currChar.direction = 9;
                //Debug.Log("UFORWARD");
                
                deltaRot = Quaternion.Euler(0, 315  , 0);
                model.MoveRotation(deltaRot);
                if (fullHoldDTimer > 0)
                {
                    break;
                }
                if (!isBlocking && recoveryFrames == 0)
                {
                    currChar.Walk(this);
                }
                if (!isBlocking && ((dashInputRecording == 9 && dashTimer != 0) || isDashing) && recoveryFrames == 0)
                {
                   // Debug.Log("UFORWARD DASH");
                    currChar.Dasher(this);
                    dashTimer = 2;
                    isDashing = true;
                }
                else
                {
                    lastMovementInput = 9;
                    isDashing = false;
                }
                break;

        }
        if(dashTimer != 0)
        {
            dashTimer--;
        }
        
        //special input processor
        switch (currMovementKey)
        {
            case 1:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                if (HCF[0])
                {
                    HCF[1] = true;
                }
                if(QCB[0])
                {
                    QCB[1] = true;
                }
                if (HCB[0] && HCB[1])
                {
                    HCB[3] = true;
                    HCB[2] = true;
                }
                else
                {
                    HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                }
             FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;
            case 2:
               
                if(HCFTimer == 0 && HCBTimer == 0)
                {
                QCF[0] = true;
                QCFTimer = 20;
                QCB[0] = true;
                QCBTimer = 20;
                }
               
                if (BDP[0])
                {
                    BDP[1] = true;
                }
               
               
                if(FDP[0])
                {
                    FDP[1] = true;
                    Debug.Log("DP2");
                }
                
                break;
            case 3:
                if (FDP[1])
                {
                    FDP[2] = true;
                    FDPTimer = 60;
                    Debug.Log("DP3");

                }
                if (FDP[0])
                {
                    FDP[1] = true;
                    FDPTimer = 60;
                    Debug.Log("DP2");
                }
                
                if (QCF[0])
                {
                    QCF[1] = true;
                }
                else
                {
                    QCF[0] = false;
                }
                BDP[0] = false;
                BDP[1] = false;
                BDP[2] = false;
                if(HCF[0] && HCF[1])
                {
                    HCF[3] = true;
                    HCF[2] = true;
                }
                else
                {
                    HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                }

                if(HCB[0])
                {
                    HCB[1] = true;
                }
                QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
                break;
            case 4:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                

                if(BDP[1] && BDP[0] && BDPTimer != 0)
                {
                    BDP[2] = true;
                }
                else
                {
                    BDPTimer = 30;
                    BDP[0] = true;
                }
                if(QCB[0] && QCB[1])
                {
                    QCB[2] = true;
                }
                if (HCB[0] && HCB[1] && HCB[2] && HCB[3] && HCBTimer != 0)
                {
                    HCB[4] = true;
                }
                else
                {
                    HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                }
                if (HCB[0] && HCB[1] && HCB[2] && HCB[3])
                {
                    HCB[4] = true;
                }
                else
                {
                    HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                }

                HCF[0] = true;
                HCFTimer = 40;
                FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;

            case 5:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                BDP[0] = false;

            BDP[1] = false;
            BDP[2] = false;
                HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                    QCB[0] = false;
                    QCB[1] = false;
                    QCB[2] = false;
                HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                 FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;
            case 6:
                if (QCF[0] && QCF[1])
                {
                    QCF[2] = true;
                }
            
                BDP[0] = false;
                BDP[1] = false;
                BDP[2] = false;
                QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
                if(FDP[0] && FDP[1] && FDPTimer != 0)
                {
                    Debug.Log("NFDP");
                    FDP[2] = true;
                    FDPTimer = 1000;
                }
                else
                {
                    Debug.Log("DP1");
                    FDPTimer = 60;
                    FDP[0] = true;
                }
                if(HCF[0] && HCF[1] && HCF[2] && HCF[3])
                {
                    HCF[4] = true;
                }
                else
                {
                    HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                }
                HCBTimer = 40;
                HCB[0] = true;  
                break;
            case 7:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
                BDP[0] = false;
            BDP[1] = false;
            BDP[2] = false;
                HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                 FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;
            case 8:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
                BDP[0] = false;
            BDP[1] = false;
            BDP[2] = false;
                HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                 FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;
            case 9:
                QCF[0] = false;
                QCF[1] = false;
                QCF[2] = false;
                QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
                BDP[0] = false;
            BDP[1] = false;
            BDP[2] = false;
                HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                 FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
                break;
        }
        switch (currInput.KEY)
        {

            case 0:
                
                if(isBlocking)
                {
                    currChar.Idle(this);
                    isBlocking = false;
                }
                
                if (fullHoldDTimer >= 60)
                {
                    Debug.Log("Full hold D release!");
                    fullHoldDTimer = 0;
                    semiHoldDTimer = 0;
                    currChar.FullHoldDAttack(this);
                    break;
                }
                
                 if ((fullHoldDTimer < 60 && semiHoldDTimer > 30))
                {
                    Debug.Log("Semi hold D");
                    fullHoldDTimer = 0;
                    semiHoldDTimer = 0;
                    currChar.SemiHoldD(this);
                    break;
                }

                
                if (fullHoldDTimer < 30 && fullHoldDTimer != 0)
                {
                    if (!isDashing && (recoveryFrames == 0 || (comboWindow && accemptableInputs == 1)) && !busy)
                    {
                        Debug.Log("Attack!");
                        currChar.Attack(this);
                        fullHoldDTimer = 0;
                        semiHoldDTimer = 0;



                        break;

                    }
                }
                break;
                    




                   
            case 1:

                if(Input.GetButton("ATTACK"))
                {
                    Debug.Log("Charging");
                    
                    fullHoldDTimer++;
                    semiHoldDTimer++;
                    if (fullHoldDTimer > 10)
                    {
                        
                            currChar.HoldDStartup(this);
                        
                    }
                    
                }
                
                if
                   (fullHoldDTimer >= 60)
                {
                    Debug.Log("Full hold D!");


                }
                if (!isGrounded)
                {
                   // Debug.Log("Jump attack!");
                    currChar.JumpAttack(this);
                    fullHoldDTimer = 0;
                    semiHoldDTimer = 0;

                    break;
                }
                if(isDashing)
                {
                    //Debug.Log("Dash attack!");
                    isDashing = false;
                    
                    currChar.isDashing = false;
                    currChar.DashAttack(this);
                    fullHoldDTimer = 0;
                    semiHoldDTimer = 0;
                    break;
                }
               







                
                
                if (QCF[2] && QCFTimer != 0 && !HCF[0])
                {
                    Debug.Log("Normal Quarter circle forward!");
                    QCF[0] = false;
                    QCF[1] = false;
                    QCF[2] = false;
                    currChar.QCF(this, false);
                    QCFTimer = 0;
                    break;
                }
                else
                if (BDP[2] && BDPTimer != 0)
                {
                    Debug.Log("Normal Backwards Dragon punch!");
                    BDP[0] = false;
                    BDP[1] = false;
                    BDP[2] = false;
                    BDPTimer = 0;
                    break;
                }
                else
                if (HCF[4] && HCFTimer != 0)
                {
                    Debug.Log("Normal Half Circle Forward!");
                    HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                    HCFTimer = 0;
                    break;
                }
                else
                if (QCB[2] && QCBTimer != 0 && !HCB[0])
                {
                    Debug.Log("Normal Quarter Circle Backward!");
                    QCB[0] = false;
                    QCB[1] = false;
                    QCB[2] = false;
                    QCBTimer = 0;
                    break;
                }
                else
                if (FDP[2] && FDPTimer != 0)
                {
                    Debug.Log("Normal Forward Dragon Punch!");
                    FDP[0] = false;
                    FDP[1] = false;
                    FDP[2] = false;
                    FDPTimer = 0;
                    currChar.NFDP(this);
                    currChar.resetStrings();
                    fullHoldDTimer = 0;
                    semiHoldDTimer = 0;
                    break;
                }
                else
                // Debug?.Log("hehexd");
                // Debug.Log($"HCB size: {HCB.Length} ({HCB.Length>0 ? HCB[0] : "null" }, {HCB.Length>1 ? HCB[1] : "null" }, { HCB.Length>2 ? HCB[2] : "null" }, {HCB.Length > 3 ? HCB[3] : "null" }), {HCB.Length > 4 ? HCB[4] : "null"})");
                //Debug.Log($"HCB size: {HCB.Length} ({HCB[0]}, {HCB[1]}, {HCB[2]}, {HCB[3]}, {HCB[4]}))");
                if (HCB[4] && HCBTimer != 0)
                {

                    //Debug.Log($"HCB size: {HCB.Length} ({HCB[0]}, {HCB[1]}, {HCB[2]}, {HCB[3]}), {HCB[4]})");
                    Debug.Log("Normal Half Circle Backward!");
                    HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                    HCBTimer = 0;
                    break;
                }
                break;
            case 2:
                
                if (QCF[2] && QCFTimer != 0 && !HCF[0])
                {
                    Debug.Log("EX Quarter circle forward!");
                    QCF[0] = false;
                    QCF[1] = false;
                    QCF[2] = false;
                    QCFTimer = 0;
                    break;
                }
                if (BDP[2] && BDPTimer != 0)
                {
                    Debug.Log("EX Backwards Dragon punch!");
                    BDP[0] = false;
                    BDP[1] = false;
                    BDP[2] = false;
                    BDPTimer = 0;
                    break;
                }
                if (HCF[4] && HCFTimer != 0)
                {
                    Debug.Log("EX Half Circle Forward!");
                    HCF[0] = false;
                    HCF[1] = false;
                    HCF[2] = false;
                    HCF[3] = false;
                    HCF[4] = false;
                    HCFTimer = 0;
                    break;
                }
                if (QCB[2] && QCBTimer != 0 && !HCB[0])
                {
                    Debug.Log("EX Quarter Circle Backward!");
                    QCB[0] = false;
                    QCB[1] = false;
                    QCB[2] = false;
                    QCBTimer = 0;
                    break;
                }
                if (FDP[2] && FDPTimer != 0)
                {
                    Debug.Log("EX Forward Dragon Punch!");
                    FDP[0] = false;
                    FDP[1] = false;
                    FDP[2] = false;
                    FDPTimer = 0;
                    break;
                }
                // Debug?.Log("hehexd");
                // Debug.Log($"HCB size: {HCB.Length} ({HCB.Length>0 ? HCB[0] : "null" }, {HCB.Length>1 ? HCB[1] : "null" }, { HCB.Length>2 ? HCB[2] : "null" }, {HCB.Length > 3 ? HCB[3] : "null" }), {HCB.Length > 4 ? HCB[4] : "null"})");
                //Debug.Log($"HCB size: {HCB.Length} ({HCB[0]}, {HCB[1]}, {HCB[2]}, {HCB[3]}, {HCB[4]}))");
                if (HCB[4] && HCBTimer != 0)
                {

                    //Debug.Log($"HCB size: {HCB.Length} ({HCB[0]}, {HCB[1]}, {HCB[2]}, {HCB[3]}), {HCB[4]})");
                    Debug.Log("EX Half Circle Backward!");
                    HCB[0] = false;
                    HCB[1] = false;
                    HCB[2] = false;
                    HCB[3] = false;
                    HCB[4] = false;
                    HCBTimer = 0;
                    break;
                    
                }
                Debug.Log("EX!");
                break;
            case 3:

                if (isGrounded)
                {
                    //Debug.Log("Jump!");
                    currChar.Jump(this);
                    isGrounded = false;
                }
                break;
                
            case 4:
                Debug.Log("Block!");
                currChar.Block(this);
                currChar.StopDashing(this);
                isDashing = false;
                break;
            case 5:
                Debug.Log("Weapon Skill!");
                break;
            case 6:
                Debug.Log("Armor Skill!");
                break;
            case 7:
                Debug.Log("Helmet Skill!");
                break;
            case 8:
                Debug.Log("Trinket Skill!");
                break;
            case 9:
                Debug.Log("Grab attack!");
                break;

        }
        
        //end input interp
        // Debug.Log(HCF[0] + " " + HCF[1] + " " + HCF[2] + " " + HCF[3] + " " + HCF[4] + " " + HCFTimer);
        // if (BDP[0]) {
        //Debug.Log(BDP[0] + " " + BDP[1] + " " + BDP[2] + BDPTimer);
        //   }
        //Debug.Log($"HCB size: {HCB.Length} ({HCB[0]}, {HCB[1]}, {HCB[2]}, {HCB[3]}, {HCB[4]}))");
        if (QCF[0])
        {
          //  Debug.Log(QCF[0] + " " + QCF[1] + " " + QCF[2] + " " + QCFTimer + " " + FDPTimer);
        }
       
        frame++;
        if (QCFTimer != 0)
        {
            QCFTimer--;
        }
        else
        {
            QCF[0] = false;
            QCF[1] = false;
            QCF[2] = false;
        }
        if (BDPTimer != 0)
        {
            BDPTimer--;
        }
        else
        {
            BDP[0] = false;
            BDP[1] = false;
            BDP[2] = false;
        }
        if(HCFTimer != 0)
        {
            HCFTimer--;
        }
        else
        {
            HCF[0] = false;
            HCF[1] = false;
            HCF[2] = false;
            HCF[3] = false;
            HCF[4] = false;
        }
        if(QCBTimer != 0)
        {
            QCBTimer--;
        }
        else
        {
            QCB[0] = false;
            QCB[1] = false;
            QCB[2] = false;
        }
        if(FDPTimer !=0)
        {
            FDPTimer--;
        }
        else
        {
            FDP[0] = false;
            FDP[1] = false;
            FDP[2] = false;
        }
       if(HCBTimer != 0)
        {
            HCBTimer--;
        }
        else
        {
            HCB[0] = false;
            HCB[1] = false;
            HCB[2] = false;
            HCB[3] = false;
            HCB[4] = false;
        }
      if(recoveryFrames != 0 )
        {
            recoveryFrames--;
        }
        else
        {
            currChar.resetStrings();
        }
        
        
    }
    public int inputProcessor(){
        if (Input.GetButton("ATTACK"))
        {
            if (Input.GetButton("BLOCK"))
            {
                if (Input.GetButton("JUMP"))
                {
                    return 6;
                }
                else
                {
                    return 5;
                }
            }
            else if(Input.GetButton("JUMP"))
            {
                return 7;
            }
            else if(Input.GetButton("EX"))
            {
                return 9;
            }
            else
            {
                return 1;
            }
        }
        else if (Input.GetButton("EX"))
        {
            return 2;
        }
        else if (Input.GetButton("JUMP"))
        {
            if (Input.GetButton("BLOCK"))
            {
                return 8;
            }
            else
            {
                return 3;
            }
        }
        else if (Input.GetButton("BLOCK"))
        {
            return 4;
        }
        else
          {
              return 0;
          }
    }
    public int movementProcessor()
    {
        if (Input.GetButton("UP"))
        {
            if (Input.GetButton("DOWN"))
            {
                return 5;
            }
            else if (Input.GetButton("LEFT"))
            {
                return 7;
            }
            else if (Input.GetButton("RIGHT"))
            {
                return 9;
            }
            else
            {
                return 8;
            }
        }
        else
        if (Input.GetButton("DOWN"))
        {
            if (Input.GetButton("UP"))
            {
                return 5;
            }
            else if (Input.GetButton("LEFT"))
            {
                return 1;
            }
            else if (Input.GetButton("RIGHT"))
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        else
        if (Input.GetButton("LEFT"))
        {
            if (Input.GetButton("RIGHT"))
            {
                return 5;
            }
            else if(Input.GetButton("UP")){
                return 7;
            }
            else if (Input.GetButton("DOWN"))
            {
                return 1;
            }
            else
            {
                return 4;
            }
        }
        else
        if (Input.GetButton("RIGHT"))
        {
            if (Input.GetButton("LEFT"))
            {
                return 5;
            }
            else if (Input.GetButton("UP"))
            {
                return 9;
            }
            else if (Input.GetButton("DOWN"))
            {
                return 3;
            }
            else
            {
                return 6;
            }
        }
        else
        {
            return 5;
        }
    }
    

  /*  public bool QCF(Inputs i)
    {
        bool forward = false;
        bool downForward = false;
        bool down = false;
        int frameCheck = i.FRAME;
        int curreMovement;
        //Variable that stores how many previous frames we'll be checking
        int previousFrames;
        //condition if arraylist is too small
        if (InputsList.Count < 4)
        {
            return false;
        }
        else
        //condition if arraylist is inbetween 4 and 30
        if(InputsList.Count >= 4 && InputsList.Count < 30)
        {
            previousFrames = InputsList.Count;

        }
            //otherwise, set previousFrames to standard 20 checks
        else
        {
            previousFrames = 30;
        }
        //next after determining how many frames we'll check back, do the check back
        int x = 1;
        while(x < previousFrames)
        {
            currInput = (Inputs)InputsList[frameCheck - x];
            int currMovement = currInput.MOVEMENT;
            if (!forward)
            {
                if(currMovement == 6)
                {
                    forward = true;
                    x++;
                }
                else
                {
                    x++;
                }
            }
            else
            if (!downForward)
            {
                //check the next one if the player was holding down forward
                if (currMovement == 6)
                {
                    x++;
                }
                    //reset the chain if we don't get downfoward or down
                else if(currMovement != 3 || currMovement != 2 )
                {
                    forward = false;
                    x++;
                }
                else
                {
                    downForward = true;
                    x++;
                }
            }
            else
            if (!down)
            {
                //same as downforward, but check for downforward rather than forward
                if (currMovement == 3)
                {
                    x++;
                }
                else if(currMovement != 2)
                {
                    downForward = false;
                    forward = false;
                }
                else
                {
                    down = true;
                    x++;
                }
            }
            else
            {
                break;
            }
        }
        if (forward && downForward && down)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/


    //Read inputs, for QCF start with 2, then wait for 3, then wait for 6 and the D/F key\
    //upon pressing down, start QCF chain, upon seeing forward and D/F key, yell 'QCF!'




}
    
