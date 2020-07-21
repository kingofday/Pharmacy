import addr from './addresses';
import strings from './../shared/constant';

export default class apiDrug {
    static async search(q) {
        let url = addr.searchDrug(q);
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            const rep = await response.json();
            if (rep.IsSuccessful)
                return {
                    success: true,
                    result: rep.Result.map((d) => ({
                        drugId: d.DrugId,
                        priceId: d.PriceId,
                        nameFa: d.NameFa,
                        nameEn: d.NameEn,
                        shortDescription:d.ShortDescription,
                        count: d.Count,
                        uniqueId: d.UniqueId,
                        discount: d.DiscountPrice,
                        price: d.Price,
                        unitName: d.UnitName,
                        thumbnailImageUrl: d.ThumbnailImageUrl
                    }))
                };
            else
                return { success: false, message: rep.Message }
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}