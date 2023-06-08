using UnityEngine;
using EnumTypes;

namespace BrokenVector.LowPolyFencePack
{
    /// <summary>
    /// This class toggles the door animation.
    /// The gameobject of this script has to have the DoorController script which needs an Animator component
    /// and some kind of Collider which detects your mouse click applied.
    /// </summary>
    [RequireComponent(typeof(DoorController))]
	public class DoorToggle : MonoBehaviour
    {

	    [SerializeField] private bool isOutDoor = false; // 바깥쪽 문인지 
	    
	    [Header("Player")]
	    [SerializeField] private Animator anim;
	    [SerializeField] private GameObject cap;

        private DoorController doorController;

        void Awake()
        {
            doorController = GetComponent<DoorController>();
        }

        private void OnTriggerEnter(Collider other)
	    {
		    GameObject triggerObject = other.gameObject;
		    
		    // 문 열림 
		    if (triggerObject.CompareTag(TagType.Player.ToString()))
		    {
			    doorController.OpenDoor();

			    if (isOutDoor)
			    {
				    if (!cap.activeSelf)
				    {
					    anim.SetTrigger(AnimType.Change.ToString());
					    cap.SetActive(true);
				    }
			    }
		    }
	    }
	    
	    private void OnTriggerExit(Collider other)
	    {
		    GameObject triggerObject = other.gameObject;

		    // 문 닫힘 
		    if (triggerObject.CompareTag(TagType.Player.ToString()))
		    {
			    doorController.CloseDoor();

			    if (!isOutDoor)
			    {
				    if (cap.activeSelf)
				    {
					    cap.SetActive(false);
				    }
			    }
		    }
	    }
    }
}