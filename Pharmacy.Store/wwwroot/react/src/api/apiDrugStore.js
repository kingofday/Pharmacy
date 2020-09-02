import addr from './addresses';
import strings from './../shared/constant';

export default class apiDrugStore {
    static async get() {
        let url = addr.getDrugStores;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message }
            else return {
                success: true,
                result: rep.Result.map((d) => ({
                    drugStoreId: d.DrugStoreId,
                    name: d.Name,
                    imageUrl: d.ImageUrl
                }))
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