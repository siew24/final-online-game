using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using Photon.Pun;

public class DialogueManager : MonoBehaviour//Pun
{
    //new PhotonView photonView;

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public GameObject dialoguePanel;

    public bool repeatable = false;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogueEnd = new();

    private int index;

    void Awake()
    {
        //photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NetworkedTriggerDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //textComponent.text = string.Empty;
        //StartDialogue();

        if (dialoguePanel.activeInHierarchy && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void TriggerDialogue()
    {
        //photonView.RPC(nameof(NetworkedTriggerDialogue), RpcTarget.AllBuffered);
        NetworkedTriggerDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        Debug.Log($"Typing:\n{lines[index]}");
        foreach (char character in lines[index].ToCharArray())
        {
            //photonView.RPC(nameof(NetworkedSetText), RpcTarget.AllBuffered, textComponent.text + character);
            NetworkedSetText(textComponent.text + character);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        //photonView.RPC(nameof(NetworkedNextLine), RpcTarget.AllBuffered);
        NetworkedNextLine();
    }

    public void SetTriggerCollider(bool value)
    {
        GetComponent<Collider>().enabled = value;
    }

    [PunRPC]
    void NetworkedTriggerDialogue()
    {
        Debug.Log("Triggering Dialogue");

        // Activate the dialogue panel
        dialoguePanel.SetActive(true);

        // Start the dialogue script
        index = 0;
        textComponent.text = string.Empty;
        StartDialogue();

        GetComponent<Collider>().enabled = false;
    }

    [PunRPC]
    void NetworkedNextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //gameObject.SetActive(false);
            dialoguePanel.SetActive(false);
            //Destroy(dialoguePanel);
            onDialogueEnd.Invoke();

            if (repeatable)
                GetComponent<Collider>().enabled = true;
        }
    }

    [PunRPC]
    void NetworkedSetText(string text)
    {
        textComponent.text = text;
    }
}
