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
                    //OUTLINE / COLOR SWAP : On remet la couleur de l'objet par défaut
                    item.GetComponent<MeshRenderer>().material.color = Color.grey;
                    item = hit.collider.GetComponent<Interactive>();
                }

                item = hit.collider.GetComponent<Interactive>();

                if( item != null )
                {
                    tooltip.text = item.interactText;

                    //OUTLINE / COLOR SWAP : On change la couleur de l'objet survolé
                    hit.collider.GetComponent<MeshRenderer>().material.color = Color.green;
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
                //OUTLINE / COLOR SWAP : On remet la couleur de l'objet par défaut
                item.GetComponent<MeshRenderer>().material.color = Color.grey;
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
