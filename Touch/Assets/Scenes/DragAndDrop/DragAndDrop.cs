using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �ϷŽű���������קui��ʵ�ַ��䵽��ʾ����
/// </summary>
public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect;//����λ��
    private Vector3 oldPosition;//��ʼλ��
    private string buttonName;//��ť����
    private Image img;//ͼƬ

    private void Awake()
    {
        img = transform.GetComponent<Image>();
        buttonName = gameObject.name;
        oldPosition = transform.position;
    }
    //����
    public void OnBeginDrag()
    {

    }
    //��ק
    public void OnDrag()
    {
        transform.position = Input.mousePosition;//λ�ø������
    }
    //�ɿ�
    public void OnEndDrag()
    {
        Rect rec = targetRect.rect;
        rec.position += new Vector2( targetRect.position.x,targetRect.position.y);

        if (rec.Contains(transform.position))
        {
            //λ��Y�ƶ�600��������������ص���ʼλ��
            transform.DOLocalMoveY(600, 1f).OnComplete(() =>
            {
                Debug.Log(buttonName);//udp���

                img.color = new Color(1, 1, 1, 0);
                transform.position = oldPosition;
                img.DOFade(1f, 1f);
            });
        }
        else
        {
            //ƽ�ƻص���ʼλ��
            transform.DOMove(oldPosition,1f);
        }
    }
}
