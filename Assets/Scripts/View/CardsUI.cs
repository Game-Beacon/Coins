using System;

public interface CardsUI
{
    void SetCardAndReturnUniqueID(Card card, CardUIInfo info);
    void RecycleCard(Guid guid);
}