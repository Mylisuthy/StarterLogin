import api from '../api/axios';

export interface MediaResponse {
    id: string;
    title: string;
    description: string;
    imageUrl?: string;
    videoUrl?: string;
    duration?: string;
    releaseDate?: string;
    rating?: string;
    genreId: string;
    genreName: string;
    contentType: string;
}

export interface GenreResponse {
    id: string;
    name: string;
    description?: string;
}

export const mediaService = {
    async getAllMedia(): Promise<MediaResponse[]> {
        const response = await api.get<MediaResponse[]>('/media');
        return response.data;
    },

    async getMediaById(id: string): Promise<MediaResponse> {
        const response = await api.get<MediaResponse>(`/media/${id}`);
        return response.data;
    },

    async searchMedia(query: string): Promise<MediaResponse[]> {
        const response = await api.get<MediaResponse[]>(`/media/search?query=${query}`);
        return response.data;
    },

    async getAllGenres(): Promise<GenreResponse[]> {
        const response = await api.get<GenreResponse[]>('/genres');
        return response.data;
    }
};
