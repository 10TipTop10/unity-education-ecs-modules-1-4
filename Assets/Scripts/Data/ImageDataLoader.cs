using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ImageDataLoader : MonoBehaviour
{
    [SerializeField] private string _downloadUrl;
    [SerializeField] private Image _loadImage;

    private void Start()
    {
        StartCoroutine(DownloadImageCoroutine());
    }

    private IEnumerator DownloadImageCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(_downloadUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.error}");
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            _loadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}
