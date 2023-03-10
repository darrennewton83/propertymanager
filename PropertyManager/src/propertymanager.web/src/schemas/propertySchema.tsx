import { literal, number, object, string, TypeOf, z } from 'zod';

export const propertySchema = z.object({
    id: z.number().nullable(),
    propertyTypeId: z.number().nullable(),
    propertyTypeName: string(),
    addressLine1: string()
        .nonempty('Line 1 is required'),
    addressLine2: string(),
    city: string()
        .nonempty('City is required'),
    region: string().nullable(),
    postalCode: string()
        .nonempty('Postcode is required'),
    purchasePrice: number().positive().nullable(),
    purchaseDate: z.date().nullable(),
    garage: z.boolean(),
    numberOfParkingSpaces: z.number().positive().nullable(),
    notes: string().nullable()
});

export type PropertyInput = TypeOf<typeof propertySchema>;