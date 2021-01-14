export interface AdminStatistics {
  allUsers: number;
  currentlyOnline: number;
  recentNewUsers: number; //last week
  allItems: number;
  activeItems: number; //not yet sold
  completedItems: number; //auction was won by someone
  terminatedItems: number; //aution not won by the time
  recentMonyFlow: number; //money earned from CompletedItems
  recentLoginCount: number; //login over last week
}
