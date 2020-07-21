const baseUrl = 'https://localhost:44328/';

const addr = {
    getCategories: `${baseUrl}Category`,
    searchDrug: (q) => `${baseUrl}drug?q=${q}`,
    getSingleDrug: (id) => `${baseUrl}drug/${id}`,
}
export default addr;