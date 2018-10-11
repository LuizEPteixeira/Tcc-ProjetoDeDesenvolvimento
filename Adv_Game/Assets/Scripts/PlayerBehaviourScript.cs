using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;
    public Transform verificaChao;
    public Transform verificaParede;
    
    private bool estaAndando;
    private bool estaNoChao;
    private bool estaNaParede;
    private bool estaVivo;
    private bool viradoParaDireita;
    private bool duploPulo;

    private float axis;
    public float velocidade;
    public float forcaPulo;
    public float raioValidaChao;
    public float raioValidaParede;

    public LayerMask solido;
              
    
    // Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();

        estaVivo = true;
        viradoParaDireita = true;
    }
	
	// Update is called once per frame
	void Update () {

        estaNoChao = Physics2D.OverlapCircle(verificaChao.position, raioValidaChao, solido);
        estaNaParede = Physics2D.OverlapCircle(verificaParede.position, raioValidaParede, solido);
        
        if (estaVivo)
        {
            

            axis = Input.GetAxisRaw("Horizontal");

            estaAndando = Mathf.Abs(axis) > 0f;

            if (axis > 0f && !viradoParaDireita)
                Flip();
            else if (axis < 0f && viradoParaDireita)
                Flip();

            if (Input.GetButtonDown("Jump") && estaNoChao)
            {
                rb.AddForce(tr.up * forcaPulo);
                duploPulo = true;
            }
                
            else if(Input.GetButtonDown("Jump") && !estaNoChao && duploPulo)
            {
                rb.AddForce(tr.up * forcaPulo);
                duploPulo = false;
            }


            
            
            Animations();

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


    void Animations()
    {
        an.SetBool("Andando", (estaNoChao && estaAndando));
        an.SetBool("Pulando", !estaNoChao);
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
