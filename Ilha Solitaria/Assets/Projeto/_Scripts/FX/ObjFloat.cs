using UnityEngine;

public class ObjFloat : MonoBehaviour
{
    private float Seno = 0.0f;
    private int vez = 0;
    private float cronometro = 0.0f;
    private float MovimentoEmX;
    private float TorqueEmZ;  // Usaremos apenas o torque no eixo Z para a rotação no 2D
    public float VelocidadeVertical = 1f;
    public float DistanciaVertical = 0.8f;
    public float VelocidadeHorizontal = 1.0f;
    public float VelocidadeDeRotacao = 0.4f;

    void Start()
    {
        MovimentoEmX = Random.Range(-.5f, .5f) * VelocidadeHorizontal;
        TorqueEmZ = Random.Range(-5.0f, 5.0f) * VelocidadeDeRotacao; // Aplica torque somente no eixo Z
        GetComponent<Rigidbody2D>().AddTorque(TorqueEmZ); // Aplica o torque no eixo Z para rotação 2D
    }

    void FixedUpdate()
    {
        if (Seno < Mathf.PI && vez == 0)
        {
            Seno += Time.deltaTime;
        }
        if (Seno >= Mathf.PI)
        {
            vez = 1;
        }
        if (Seno <= 0)
        {
            vez = 0;
        }
        if (Seno >= 0 && vez == 1)
        {
            Seno = 0;
        }

        // Movimento vertical e horizontal
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(MovimentoEmX, Mathf.Sin(2 * Seno * VelocidadeVertical) * DistanciaVertical);

        // Torne o cronômetro reiniciar a cada 10 segundos e adicione torque novamente
        if (cronometro < 10)
        {
            cronometro += Time.deltaTime;
        }
        if (cronometro >= 10)
        {
            cronometro = 0;
            GetComponent<Rigidbody2D>().AddTorque(TorqueEmZ);  // Aplica torque novamente a cada 10 segundos
        }
    }
}
