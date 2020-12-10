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
