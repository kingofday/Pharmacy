import addr from './addreses';
import strings from './../shared/constant';

export default class storeApi {
    static async getSingleStore(id) {
        let url = `${addr.getSingleStore}?id=${id}`;
        try {
            console.log('starting store api');
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            console.log('not reached');
            const rep = await response.json();
            if (!rep.IsSuccessful) return {
                success: false,
                message: rep.Message
            }

            else return {
                success: true,
                result: {
                    name: rep.Result.Name,
                    logoUrl: rep.Result.LogoUrl
                }
            }

        }
        catch (error) {
            if ('caches' in window)
            {
                let data = await caches.match(url);
                if (data){
                    const rep = await data.json();
                    return {
                        success: true,
                        result: {
                            name: rep.Result.Name,
                            logoUrl: rep.Result.LogoUrl
                        }
                    };
                } 
                else return ({ success: false, message: strings.connecttionFailed });
            }
            else return ({ success: false, message: strings.connecttionFailed });
        }

    }
}