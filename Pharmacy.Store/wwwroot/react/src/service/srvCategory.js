//import strings from './../shared/constant';
import apiCategory from './../api/apiCategory';
import strings from './../shared/constant';

export default class categorySrv {
    static catKey = 'categories';
    static async get(ignoreCache = false) {
        if (!localStorage) return { success: false, message: strings.useModernBrowser };
        let dt = new Date();
        if (!ignoreCache) {
            let jsonCategories = localStorage.getItem(this.catKey);
            if (jsonCategories) {
                let model = JSON.parse(jsonCategories);
                if (model.expDT > dt.getTime())
                    return {
                        success: true,
                        result: model.result
                    };
            }
        }
        let rep = await apiCategory.get();
        if (rep.success) {
            dt.setHours(dt.getHours() + 2);
            localStorage.setItem(this.catKey, JSON.stringify({
                expDT: dt.getTime(),
                result: rep.result
            }));
        }
        return rep;
    }
}