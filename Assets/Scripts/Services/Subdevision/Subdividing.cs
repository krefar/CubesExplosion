using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class Subdividing : MonoBehaviour
{
    [SerializeField] private int _minCount = 2;
    [SerializeField] private int _maxCount = 6;
    [SerializeField] private Explosion _explosion;

    private float _divideChance = 1.0f;
    private float _explosionEnd = 0.12f;

    public void Init(float divideChance = 1.0f)
    {
        _divideChance = divideChance;
    }

    public void Divide()
    {
        if (_divideChance >= Random.value)
        {
            var resultCount = Random.Range(_minCount, _maxCount + 1);
            CreateObjects(resultCount);
        }
        else
        {
            var explosion = CreateExplosion();

            StartCoroutine(ExplosionEnd(explosion));
        }

        HideGameObject();
    }

    private IEnumerator ExplosionEnd(Explosion explosion)
    {
        yield return new WaitForSeconds(_explosionEnd);

        Destroy(explosion.gameObject);
        Destroy(gameObject);
    }

    private Explosion CreateExplosion()
    {
        var explosion = Instantiate(_explosion);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = transform.localScale;

        return explosion;
    }

    private void CreateObjects(int count)
    {
        while (count-- > 0)
        {
            var newObject = Instantiate(gameObject);

            EnsureProps(newObject);
        }
    }

    private void HideGameObject()
    {
        var meshRenedrer = gameObject.GetComponent<MeshRenderer>();
        meshRenedrer.enabled = false;
        
        var collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }

    private void EnsureProps(GameObject newObject)
    {
        newObject.transform.localScale = transform.localScale / 2;

        var subdivided = newObject.GetComponent<Subdividing>();
        subdivided.Init(_divideChance / 2);

        var renderer = newObject.GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV();
    }
}