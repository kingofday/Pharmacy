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

export function ChangedBasketItemsAction(items) {
    return {
        type: actionTypes.CHANGED_BASKET_ITEMS,
        payload: { items }
    };
};

export function RemoveFromBasketAction(id) {
    return {
        type: actionTypes.REMOVE_FROM_BASKET,
        payload: { id:id }
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

export function SetPrescriptiontIdAction(id) {
    return {
        type: actionTypes.SET_PRESCRIPTION_ID,
        payload: {
            prescriptionId: id
        }
    };
};
export function ClearPrescriptiontIdAction() {
    return {
        type: actionTypes.CLEAR_PRESCRIPTION_ID
    };
};