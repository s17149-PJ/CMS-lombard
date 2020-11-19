export interface LombardProduct {
  name: string;
  category: LompardProductCategory;
  publishDate: number;
  expirationDate: number;
}

export interface LompardProductCategory {
  name: string;
}
