import addr from './addresses';
import strings from './../shared/constant';
import { cacheData } from './../shared/utils';

export default class apiDrug {
    static async get(filter) {
        let url = addr.searchDrug(filter);
        var handleResponse = async (response, isOffline) => {
            let c = response.clone();
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            cacheData(url, c);
            return {
                success: true,
                result: {
                    maxPrice: rep.Result.MaxPrice,
                    lastPageNumber: Math.floor(rep.Result.TotalCount / 9) + ((rep.Result.TotalCount % (filter.pageSize || 9)) > 0 ? 1 : 0),
                    items: rep.Result.Items.map((d) => ({
                        drugId: d.DrugId,
                        nameFa: d.NameFa,
                        nameEn: d.NameEn,
                        shortDescription: d.ShortDescription,
                        count: d.Count,
                        uniqueId: d.UniqueId,
                        discount: d.DiscountPrice,
                        price: d.Price,
                        realPrice: d.Price - d.DiscountPrice,
                        unitName: d.UnitName,
                        thumbnailImageUrl: isOffline ? process.env.PUBLIC_URL + '/offline-drug.png' : d.ThumbnailImageUrl
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
                    return await handleResponse(data, true);
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
            console.log(p);
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
                    description: p.Description,
                    slides: p.Slides,
                    properties: p.Properties ? p.Properties.map(prop => ({
                        name: prop.Name,
                        value: prop.Value
                    })) : [],
                    comments: p.Comments ? p.Comments.map(c => ({
                        comment: c.Comment,
                        fullname: c.Fullname
                    })) : [],
                    tags: p.Tags ? p.Tags.map(t => ({
                        name: t.Name,
                        id: t.TagId
                    })) : []

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