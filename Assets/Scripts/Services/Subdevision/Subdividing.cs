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
    private float _explosionEnd = 0.16f;

    public void Init(float divideChance = 1.0f)
    {
        _divideChance = divideChance;
    }

    public void Divide()
    {
        if (_divideChance >= Random.value)
        {
            var resultCount = Random.Range(_minCount, _maxCount + 1);
            var objects = CreateObjects(resultCount);
            
            HideGameObject();

            var explosion = CreateExplosion();

            StartCoroutine(ExplosionEnd(objects, explosion));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ExplosionEnd(List<GameObject> objects, Explosion explosion)
    {
        yield return new WaitForSeconds(_explosionEnd);

        MoveToCubesLayer(objects);
        Destroy(explosion.gameObject);
        Destroy(gameObject);
    }

    private void  MoveToCubesLayer(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.layer = LayerMask.NameToLayer(Layers.Cubes.ToString());
        }
    }

    private Explosion CreateExplosion()
    {
        var explosion = Instantiate(_explosion);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = transform.localScale;

        return explosion;
    }

    private List<GameObject> CreateObjects(int count)
    {
        var result = new List<GameObject>();

        while (count-- > 0)
        {
            var newObject = Instantiate(gameObject);

            EnsureProps(newObject);

            result.Add(newObject);

        }

        return result;
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
        newObject.layer = LayerMask.NameToLayer(Layers.Explosion.ToString());

        var subdivided = newObject.GetComponent<Subdividing>();
        subdivided.Init(_divideChance / 2);

        var renderer = newObject.GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV();
    }
}