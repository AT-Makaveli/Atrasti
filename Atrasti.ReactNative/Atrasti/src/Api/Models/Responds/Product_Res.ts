

export interface Product_Res {
    id: number;
    title: string;
    description: string;
    tags: string[];
    productLikes: number[];
    isHeartPressed: boolean;
    userId: number;
}