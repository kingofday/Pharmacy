import addr from './addresses';
import strings from './../shared/constant';
import { cacheData } from './../shared/utils';

export default class apiDrugStore {
    static async get() {
        let url = addr.getDrugStores;
        var handleResponse = async (response, isOffline) => {
            let c = response.clone();
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            cacheData(url, c);
            return {
                success: true,
                result: rep.Result.map((d) => ({
                    drugStoreId: d.DrugStoreId,
                    name: d.Name,
                    imageUrl: isOffline ? process.env.PUBLIC_URL + '/pharmacy.png' : d.ImageUrl
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
                    return await handleResponse(data, true);
                else return ({ success: false, message: strings.connectionFailed });
            }
            else return ({ success: false, message: strings.connectionFailed });
        }
    }
}