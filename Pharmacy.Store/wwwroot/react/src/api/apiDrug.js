import addr from './addresses';
import strings from './../shared/constant';

export default class apiDrug {
    static async get(filter) {
        let url = addr.searchDrug(filter);
        console.log(url);
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            else return {
                success: true,
                result: {
                    maxPrice: rep.Result.MaxPrice,
                    lastPageNumber: Math.floor((rep.Result.TotalCount / 9) + (rep.Result.TotalCount % filter.pageSize === 0 ? 0 : 1)),
                    items: rep.Result.Items.map((d) => ({
                        drugId: d.DrugId,
                        priceId: d.PriceId,
                        nameFa: d.NameFa,
                        nameEn: d.NameEn,
                        shortDescription: d.ShortDescription,
                        count: d.Count,
                        uniqueId: d.UniqueId,
                        discount: d.DiscountPrice,
                        price: d.Price,
                        unitName: d.UnitName,
                        thumbnailImageUrl: d.ThumbnailImageUrl
                    }))
                }
            }
        }
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            if ('caches' in window) {
                let data = await caches.match(url);
                if (data)
                    return await handleResponse(data);
                else return ({ success: false, message: strings.connectionFailed });
            }
            else return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async getSingle(id) {
        let url = addr.getSingleDrug(id);
        console.log(url);
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            const p = rep.Result;
            return {
                success: true,
                result: {
                    drugId: p.DrugId,
                    price: p.Price,
                    discount: p.DiscountPrice,
                    realPrice: p.Price - p.DiscountPrice,
                    nameFa: p.NameFa,
                    nameEn: p.NameEn,
                    categoryName: p.CategoryName,
                    unitName: p.UnitName,
                    uniqueId: p.UniqueId,
                    slides: p.Slides,
                    tags: p.Tags
                }
            }
        }
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            if ('caches' in window) {
                let data = await caches.match(url);
                if (data)
                    return await handleResponse(data);
                else return ({ success: false, message: strings.connectionFailed });
            }
            else return ({ success: false, message: strings.connectionFailed });
        }
    }
}