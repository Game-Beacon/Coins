public interface CardsUI
{
    int SetCardAndReturnUniqueID(Card card, CardUIInfo info);
    void RecycleCard(int guid);
}