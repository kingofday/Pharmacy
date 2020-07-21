import addr from './addreses';
export default class userApi {
    static signUp = (user) => new Promise((resolve) => {
        //simulate api
        setTimeout(function () {
            resolve({
                success: true,
                result: {
                   token:'123-456'
                }
            });
        }, 1000)

    });
}