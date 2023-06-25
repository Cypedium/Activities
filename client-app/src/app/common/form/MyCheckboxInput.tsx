import { useField } from 'formik';
import React from 'react';
import { Form, FormCheckbox, Label, Select } from 'semantic-ui-react';

interface Props {
    name: string;
    checked: boolean;
    label: string;
}

export default function MyCheckboxInput(props: Props) {
    const [field, meta, helpers] = useField(props.name);
    return (
        <Form.Field error={meta.touched && !!meta.error}> {/* !!cast to boolean */}
            <label>{props.label}</label>
           <FormCheckbox
             clearable
             value={field.value || null}
             onChange={(event, data) => helpers.setValue(data.value)}
             onBlur={() => helpers.setTouched(true)}
             label={props.label}
             checked={props.checked}   
           />
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}