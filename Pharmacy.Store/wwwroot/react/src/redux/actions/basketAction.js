import actionTypes from './actionTypes';

export function AddToBasketAction(item, count) {
    return {
        type: actionTypes.ADD_TO_BASKET,
        payload: { ...item, count }
    };
};

export function UpdateBasketAction(drugId, count) {
    return {
        type: actionTypes.UPDATE_BASKET,
        payload: { drugId, count }
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

export function SetWholeBasketAction(items) {
    return {
        type: actionTypes.SET_WHOLE,
        payload: { items }
    };
};