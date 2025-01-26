using UnityEngine;

public class ManualClockMob : ClockMob
{

    private float currentDegree;
    
    void Update()
    {
        IncreaseProgress();
        RotatePointer();
    }
    
    public void Hit(float degree)
    {
        currentDegree += degree;
        if(currentDegree >= 360)
            OnComplete?.Invoke();
    }

    private void IncreaseProgress()
    {
        var value = Mathf.Lerp(progressMat.GetFloat("_Arc2"), 360 - currentDegree, Time.deltaTime * 2f);
        progressMat?.SetFloat("_Arc2", value);
    }

    private void RotatePointer()
    {
        Quaternion targetRotation = Quaternion.Euler(pointer.localRotation.eulerAngles.x,
            pointer.localRotation.eulerAngles.y, -currentDegree);
        pointer.localRotation = Quaternion.Lerp(pointer.localRotation, targetRotation, Time.deltaTime * 2f);
    }
}
