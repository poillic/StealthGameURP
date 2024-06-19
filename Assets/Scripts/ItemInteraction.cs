using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] InputAction interactAction;
    [SerializeField] float interactionMaxDistance;
    [SerializeField] LayerMask InteractiveLayer;
    [SerializeField] Color crossHairOff;
    [SerializeField] Color crossHairOn;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Image crosshair;
    [SerializeField] private TextMeshProUGUI tooltip;

    private Transform mainCamera;

    private void OnEnable()
    {
        interactAction.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if( Physics.Raycast( mainCamera.position, mainCamera.forward, out RaycastHit hit, Mathf.Infinity, InteractiveLayer ) )
        {
            if( Vector3.Distance(playerTransform.position, hit.point ) <= interactionMaxDistance  )
            {
                crosshair.color = crossHairOn;

                //Si l'objet que l'on survole est différent du précédent
                if( item != null && hit.collider.GetComponent<Interactive>() != item )
                {
                    item = hit.collider.GetComponent<Interactive>();
                }

                item = hit.collider.GetComponent<Interactive>();

                if( item != null )
                {
                    tooltip.text = item.interactText;
                    if ( interactAction.WasPerformedThisFrame() )
                    {
                        item.Use();
                    }
                }
            }
            else
            {
                //On ne survole pas d'objet
                TurnOff();
            }
        }
        else
        {
            //On ne survole pas d'objet donc on remet l'objet que l'on survole à null
            if( item != null )
            {
                item = null;
            }

            //On ne survole pas d'objet
            TurnOff();
        }
    }

    private void TurnOff()
    {
        crosshair.color = crossHairOff;
        tooltip.text = "";
    }

    private Interactive item;
}
