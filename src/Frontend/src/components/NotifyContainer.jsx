import React from 'react';
import { useFieldArray, useFormContext, useWatch } from 'react-hook-form';
import { NotifyRecord } from './NotifyRecord';

export const NotifyContext = React.createContext({
    remove: () => { },
    append: () => { }
});

export const NotifyContainer = props => {
    const { control } = useFormContext();

    const notificationName = useWatch({
        name: `notifications.${props.socialIndex}.notification`,
        control
    });

    const { fields, remove, append } = useFieldArray({
        control,
        receiver: `notifications.${props.socialIndex}.receiver`,
        message: `notifications.${props.socialIndex}.message`
    });

    const addProfileHandler = () => {
        append({
            receiver: '',
            message: ''
        });
    };

    return (
        <div>
            <NotifyContext.Provider value={{ remove }}>
                <h1>{notificationName}</h1>
                {fields.map((field, index) => (
                    <NotifyRecord
                        index={index}
                        receiver={`socials.${props.socialIndex}.profiles.${index}.name`}
                        key={field.id}
                    />
                ))}
                <button type="button" onClick={addProfileHandler}>
                    Add profile
                </button>
            </NotifyContext.Provider>
        </div>
    );
};
