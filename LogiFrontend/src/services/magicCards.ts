import api from '../api/axios';

export interface MagicCard {
    id: string;
    title: string;
    description: string;
    imageUrl: string;
    isPublished: boolean;
    createdAt: string;
}

export const magicCardsApi = {
    getAll: async (includeDrafts = false): Promise<MagicCard[]> => {
        const { data } = await api.get('/magiccards', { params: { includeDrafts } });
        return data;
    },

    create: async (formData: FormData): Promise<string> => {
        const { data } = await api.post('/magiccards', formData, {
            headers: { 'Content-Type': 'multipart/form-data' }
        });
        return data.id;
    },

    update: async (id: string, formData: FormData): Promise<void> => {
        await api.put(`/magiccards/${id}`, formData, {
            headers: { 'Content-Type': 'multipart/form-data' }
        });
    },

    publish: async (id: string, isPublished: boolean): Promise<void> => {
        await api.put(`/magiccards/${id}/publish`, { isPublished });
    },

    delete: async (id: string): Promise<void> => {
        await api.delete(`/magiccards/${id}`);
    }
};
