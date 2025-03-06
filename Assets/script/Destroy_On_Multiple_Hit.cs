using UnityEngine;
using UnityEngine.Audio;

public class Destroy_On_Multiple_Hit : MonoBehaviour
{
    [SerializeField] int maxHitCount = 10;
    [SerializeField] private bool randomHitCount = true;

    Material _material;
    private float _destroyStepsPercent = 1;

    GameMenager _gameMenager;
    public GameMenager GameMenager { set => _gameMenager = value; }

    private AudioSource _audioSource;
    
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _audioSource = GetComponent<AudioSource>();

        if (randomHitCount)
        {
            maxHitCount = Random.Range(1, maxHitCount);
        }

        _destroyStepsPercent = 1f/maxHitCount;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponent<Mouve_Bullets>()) return;

        maxHitCount -= 1;
        _material.color = new Color(0,0,0, _destroyStepsPercent);

        Debug.Log($"{_destroyStepsPercent} -> alpha = {_material.color.a}");

        if (maxHitCount <= 0)
        {
            _gameMenager.DidDestroyWall();

            if (_audioSource.clip)
            {
                _audioSource.Play();
                Invoke(nameof(DestroyMe), _audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void DestroyMe()
    {
        _gameMenager = null;
        Destroy(gameObject);
    }
}
