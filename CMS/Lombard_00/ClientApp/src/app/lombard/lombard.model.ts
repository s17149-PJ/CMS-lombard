export interface LombardProduct {
  id: number;
  name: string;
  category: LompardProductCategory;
  publishDate: number;
  expirationDate: number;
  description: string;
  img?: string;
}

export interface LompardProductCategory {
  name: string;
}
