import { useFormContext } from 'react-hook-form';
import React, { useContext } from 'react';
import { SocialContext } from './SocialContainer';

export const NotifyRecord = props => {
    const { register } = useFormContext();
    const notifyContext = useContext(NotifyContext);

    const deleteHandler = () => notifyContext.remove(props.profileIndex);

    return (
        <div>
            <input type="text" {...register(props.name)} />
            <button type="button" onClick={deleteHandler}>
                Delete
            </button>
        </div>
    );
};
