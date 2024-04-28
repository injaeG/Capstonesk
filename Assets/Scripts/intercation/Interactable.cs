public interface Interactable
{
    bool IsInteracting { get; }
    void Interact();
    void ResetInteraction();
    void SecondaryInteract(); // 새로 추가된 메소드
}
