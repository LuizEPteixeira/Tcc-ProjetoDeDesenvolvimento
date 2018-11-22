using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;

    [Header("Verificadores")]
    [SerializeField] private Transform verificaChao;
    [SerializeField] private Transform verificaParede;
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private bool estaAndando;
    private bool estaNoChao;
    private bool estaNaParede;
    private bool estaVivo;
    private bool viradoParaDireita;
    private bool pulo;
    private bool duploPulo;
    private float axis;

    [Header("Jogador")]
    public float velocidade;
    public float forcaPulo;
    public float raioValidaChao;
    public float raioValidaParede;
    public Joystick joystick;
    public FixedButton jumpButton;

    [Header("Layers")]
    public LayerMask solido;
    public LayerMask water;
    public LayerMask enemy;


    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();

        estaVivo = true;
        viradoParaDireita = true;

        player.transform.position = respawnPoint.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        estaNoChao = Physics2D.OverlapCircle(verificaChao.position, raioValidaChao, solido);
        estaNaParede = Physics2D.OverlapCircle(verificaParede.position, raioValidaParede, solido);
        
        if (estaVivo)
        {
            

            axis = joystick.Horizontal;
            pulo = jumpButton.Pressed;

            estaAndando = Mathf.Abs(axis) > 0f;

            if (axis > 0f && !viradoParaDireita)
                Flip();
            else if (axis < 0f && viradoParaDireita)
                Flip();

            if (pulo && estaNoChao)
            {
                rb.AddForce(tr.up * forcaPulo);
                duploPulo = true;
                
            }
                
            else if(pulo && !estaNoChao && duploPulo)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(tr.up * forcaPulo);
                duploPulo = false;
            }


            if (rb.IsTouchingLayers(water) || rb.IsTouchingLayers(enemy))
            {
                PlayerDie();
                
            }
            
            
            PlayerAnimations();
            

        }

        

    }


    private void FixedUpdate(){
        

        if (estaAndando && !estaNaParede)
        {

            if (viradoParaDireita)
                rb.velocity = new Vector2(velocidade, rb.velocity.y);
            else
                rb.velocity = new Vector2(-velocidade, rb.velocity.y);
            
        }

        
    }

    void Flip() {

        viradoParaDireita = !viradoParaDireita;

        tr.localScale = new Vector2(-tr.localScale.x, tr.localScale.y);
        
    }


    void PlayerDie()
    {

        estaVivo = false;
        StartCoroutine("TimeWaitDeath");
    }



    IEnumerator TimeWaitDeath()
    {
        
        yield return new WaitForSecondsRealtime(0.5f);
        Reviver();

    }


    void Reviver()
    {

        player.transform.position = respawnPoint.transform.position;
        an.Rebind();
        estaVivo = true;

    }


    void PlayerAnimations()
    {
        an.SetBool("Andando", (estaNoChao && estaAndando && estaVivo));
        an.SetBool("Pulando", (!estaNoChao && estaVivo));
        an.SetFloat("VelVertical", rb.velocity.y);
        an.SetBool("Morrendo", !estaVivo);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(verificaChao.position, raioValidaChao);
        Gizmos.DrawWireSphere(verificaParede.position, raioValidaParede);
    }


    


}
