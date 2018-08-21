using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour {

    public Sprite sprite;
    public Material material;
    [ContextMenu("Create !")]
    public void CreateMesh()
    {
        for(int x = 0; x < sprite.texture.width; x++)
        {

            for (int y = 0; y < sprite.texture.width; y++)
            {
                if(sprite.texture.GetPixel(x,y).a > 0.125f)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Destroy(cube.GetComponent<BoxCollider>());
                    cube.GetComponent<Renderer>().material = material;
                    cube.transform.position = Vector3.right * x + Vector3.up * y;
                }
            }
        }
    }

}
