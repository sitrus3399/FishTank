using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    [Header("Page")]
    [SerializeField] protected string type;
    [SerializeField] private Vector3 pointShow;
    [SerializeField] private Vector3 pointHide;
    [SerializeField] private float speed = 2.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float lerpProgress = 0f;

    public bool isShow;
    public bool isStatic;

    protected PageManager pageManager;

    public string Type { get { return type; } }

    protected virtual void OnEnable()
    {
        pageManager = PageManager.Instance; //why its not work?
    }

    protected virtual void Start()
    {
        pageManager = PageManager.Instance;
    }

    protected virtual void Update()
    {
        if (isStatic)
        {
            return;
        }

        if ((isShow && targetPosition != pointShow) || (!isShow && targetPosition != pointHide))
        {
            startPosition = transform.localPosition;
            targetPosition = isShow ? pointShow : pointHide;
            lerpProgress = 0f;
        }

        if (lerpProgress < 1f)
        {
            lerpProgress += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpProgress);
        }
    }

    public virtual void Show()
    {
        isShow = true;
    }

    public virtual void Hide()
    {
        isShow = false;
    }

    protected virtual void OpenPage()
    {
        PageManager.Instance.OpenPage(type);
        AudioManager.Instance.PlaySFX("Click");
    }
}

public class DummyPage : Page
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}