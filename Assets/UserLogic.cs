using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Users {
    User1,
    User2
}

public class UserLogic : MonoBehaviour
{
    public Users cur_user = Users.User1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
