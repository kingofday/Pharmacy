import addr from './addreses';
import strings from './../shared/constant';

export default class productApi {
    static async getProducts(storeId, category, pageNumber) {
        let url = `${addr.getProducts}?storeId=${storeId}&category=${category}&pageNumber=${pageNumber}&pageSize=8`;
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            const rep = await response.json();
            console.log(rep);
            if (!rep.IsSuccessful)
                return {
                    success: false,
                    message: rep.Message
                }
            else
                return {
                    success: true,
                    result: rep.Result.Items.map((p) => ({
                        id: p.Id,
                        name: p.Name,
                        price: p.Price,
                        disount: p.Discount,
                        realPrice: p.RealPrice,
                        currency: p.Currency,
                        imgUrl: p.ImageUrl,
                    }))
                }
        } catch (error) {
            if ('caches' in window) {
                let data = await caches.match(url);
                if (data) {
                    const rep = await data.json();
                    return {
                        success: true,
                        result: rep.Result.Items.map((p) => ({
                            id: p.Id,
                            name: p.Name,
                            price: p.Price,
                            disount: p.Discount,
                            realPrice: p.RealPrice,
                            currency: p.Currency,
                            imgUrl: p.ImageUrl,
                        }))
                    };
                }
                else return ({ success: false, message: strings.connecttionFailed });
            }
            else return ({ success: false, message: strings.connecttionFailed });
        }
    }

    static async getSingleProduct(id) {
        let url = `${addr.getSingleProduct}?id=${id}`;
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            const rep = await response.json();
            console.log(rep);
            if (!rep.IsSuccessful)
                return {
                    success: false,
                    message: rep.Message
                }
            else
                return {
                    success: true,
                    result: {
                        id: rep.Result.Id,
                        name: rep.Result.Name,
                        price: rep.Result.Price,
                        discount: rep.Result.Discount,
                        realPrice: rep.Result.RealPrice,
                        currency: rep.Result.Currency,
                        description: rep.Result.Description,
                        slides: rep.Result.Slides ? rep.Result.Slides : [],
                    }
                }
        } catch (error) {
            if ('caches' in window) {
                let data = await caches.match(url);
                if (data) {
                    const rep = await data.json();
                    return {
                        success: true,
                        result: rep.Result.Items.map((p) => ({
                            id: p.Id,
                            name: p.Name,
                            price: p.Price,
                            disount: p.Discount,
                            realPrice: p.RealPrice,
                            currency: p.Currency,
                            imgUrl: p.ImageUrl,
                        }))
                    };
                }
                else return ({ success: false, message: strings.connecttionFailed });
            }
            else return ({ success: false, message: strings.connecttionFailed });
        }

    }

    static toggleLike = (id) => new Promise((resolve) => {
        setTimeout(function () {
            resolve({
                success: true
            });
        }, 3000)

    });

    static toggleFavorite = (id) => new Promise((resolve) => {
        setTimeout(function () {
            resolve({
                success: true
            });
        }, 3000)

    });
}