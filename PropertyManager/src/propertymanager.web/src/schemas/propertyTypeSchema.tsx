import { literal, number, object, string, TypeOf, z } from 'zod';

export const propertyTypeSchema = z.object(
    {
        id: z.number().positive(),
        name: string().nonempty()
    }
)

export type PropertyType = TypeOf<typeof propertyTypeSchema>;