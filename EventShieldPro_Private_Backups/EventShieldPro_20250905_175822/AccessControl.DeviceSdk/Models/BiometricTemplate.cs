namespace AccessControl.DeviceSdk.Models;

public sealed class BiometricTemplate
{
    public BiometricTemplate(byte[] data, BiometricTemplateType templateType)
    {
        Data = data;
        TemplateType = templateType;
    }

    public byte[] Data { get; }
    public BiometricTemplateType TemplateType { get; }
}

public enum BiometricTemplateType
{
    Palm,
    Fingerprint
}
