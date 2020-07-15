import actionTypes from './actionTypes';

export function AddToBasketAction(product, count) {
    return {
        type: actionTypes.ADD_TO_BASKET,
        payload: {
            id: product.id,
            name: product.name,
            price: product.price,
            discount: product.discount,
            realPrice: product.realPrice,
            imgUrl: product.slides.length > 0 ? product.slides[0].imgUrl : null,
            count,
        }
    };
};

export function UpdateBasketAction(id, count) {
    return {
        type: actionTypes.UPDATE_BASKET,
        payload: { id, count }
    };
};

export function ChangedBasketItemsAction(products) {
    return {
        type: actionTypes.CHANGED_BASKET_ITEMS,
        payload: { products }
    };
};

export function RemoveFromBasketAction(id) {
    return {
        type: actionTypes.REMOVE_FROM_BASKET,
        payload: { id }
    };
};


export function ClearBasketAction() {
    return {
        type: actionTypes.CLEAR_BASKET,
        payload: {}
    };
};

export function SetBasketRouteAction(route) {
    return {
        type: actionTypes.SET_BASKET_ROUTE,
        payload: { route }
    };
};

export function SetWholeBasketAction(items) {
    return {
        type: actionTypes.SET_WHOLE,
        payload: { items }
    };
};