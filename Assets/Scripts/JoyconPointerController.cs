using UnityEngine;

public class JoyconPointerController : MonoBehaviour
{
    Joycon joycon;

    [Header("���x�i��[��������ʍ��W�j")]
    public float sensitivity = 8.0f;   // �傫���قǓ���
    public float smooth = 12f;         // �傫���قǊ��炩

    [Header("��ʓ��ɐ����i���[���h���W�j")]
    public Vector2 minLimit = new Vector2(-8f, -4.5f);
    public Vector2 maxLimit = new Vector2(8f, 4.5f);

    [Header("�c�����␳")]
    public bool invertX = false;
    public bool invertY = true;        // �c�������Ə㉺���t�ɂȂ�₷���̂ŏ���true����

    Quaternion baseRot = Quaternion.identity;   // R�Ŋ
    Vector3 velocity = Vector3.zero;
    Vector3 startPos;

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con ��������܂���");
            enabled = false;
            return;
        }

        joycon = JoyconManager.Instance.j[0];
        startPos = transform.position;

        // �ŏ���1��Z���^�[������Ă���
        baseRot = joycon.GetVector();
    }

    void Update()
    {
        if (joycon == null) return;

        // R�iSHOULDER_1�j�ŃZ���^�[�i��p���j���Ď擾���|�C���^������
        if (joycon.GetButtonDown(Joycon.Button.SHOULDER_1))
        {
            baseRot = joycon.GetVector();
            transform.position = startPos;
            velocity = Vector3.zero;
            return;
        }

        // ���݂̎p��
        Quaternion rot = joycon.GetVector();

        // ��p������̑��Ή�]�iR�ŃZ���^�[�j
        Quaternion rel = Quaternion.Inverse(baseRot) * rot;

        // Joy-Con�́u��[�����v�����߂�
        // �����̓��C�u����/�������Ŏ����Y����̂Œ����|�C���g
        // rel * Vector3.forward ����{�ɁA����Ȃ���� up/right ������
        Vector3 dir = rel * Vector3.forward;

        // ��ʕ��ʂւ̓��e�Fx,y�������g���č��W�ɂ���
        float x = dir.x * sensitivity;
        float y = dir.y * sensitivity;

        if (invertX) x *= -1f;
        if (invertY) y *= -1f;

        Vector3 targetPos = new Vector3(startPos.x + x, startPos.y + y, 0f);

        // ��ʊO�ɏo�Ȃ��悤����
        targetPos.x = Mathf.Clamp(targetPos.x, minLimit.x, maxLimit.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minLimit.y, maxLimit.y);

        // �X���[�Y�ړ�
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            1f / smooth
        );
    }
}
