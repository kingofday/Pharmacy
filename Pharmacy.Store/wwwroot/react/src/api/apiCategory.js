import addr from './addresses';
import strings from './../shared/constant';
import { cacheData } from './../shared/utils';
export default class apiCategory {
    static async get(parentId = 0) {
        let url = addr.getCategories(parentId);
        var handleResponse = async (response) => {
            let c = response.clone();
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            cacheData(url, c);
            return {
                success: true,
                result: rep.Result.map((c) => ({
                    categoryId: c.CategoryId,
                    name: c.Name
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