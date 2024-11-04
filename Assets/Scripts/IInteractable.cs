public interface IInteractable
{
    string GetInteractionText();
    bool IsAvaliable(Player player);
    void Interact(Player player);

}
