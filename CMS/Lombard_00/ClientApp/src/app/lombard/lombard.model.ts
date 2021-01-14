export interface LombardProduct {
  id: number;
  name: string;
  category: LompardProductCategory;
  publishDate: number;
  finallizationDateTime: number;
  finallizationDateTimeDouble: number;
  description: string;
  image?: string;
}

export interface LompardProductCategory {
  name: string;
}

export interface Bid {
  userId: number;
  token: string;
  subjectId: number;
  money: number;
  isRating: boolean;
}
