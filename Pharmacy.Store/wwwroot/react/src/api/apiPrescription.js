import addr, { imagePrefixUrl } from './addresses';
import strings from './../shared/constant';

export default class apiPrescription {

    static async submit(token, mobileNumber, files) {
        try {
            let data = new FormData();
            let headers = {};
            if (token) headers['Authorization'] = `Bearer ${token}`;
            else data.append('mobileNumber', mobileNumber);
            files.forEach((file) => data.append('files', file, file.name));
            const response = await fetch(addr.addPrescription, {
                method: 'POST',
                mode: 'cors',
                headers: headers,
                body: data
            });
            const rep = await response.json();
            console.log(rep);
            if (rep.IsSuccessful)
                return {
                    status: 200,
                    success: true,
                    result: rep.Result
                };
            else return { success: false, message: rep.Message, status: rep.Status };
        }
        catch (error) {
            console.log(error);
            return { success: false, message: strings.connecttionFailed, status: 500 };
        }
    }
    static async getItems(id) {
        let url = addr.getPrescription(id);
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: rep.Result.map((d) => ({
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
                    thumbnailImageUrl: imagePrefixUrl + d.ThumbnailImageUrl
                }))
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'GET',
                'mode': 'cors',
                //'credentials': 'include',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}