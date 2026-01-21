from functools import lru_cache

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from transformers import AutoTokenizer

app = FastAPI()

MODEL_NAME_BY_KEY = {
    "bert": "bert-base-uncased",
    "gpt-2": "gpt2",
    "xml-roberta": "xlm-roberta-base",
    "llama": "huggyllama/llama-7b",
}

class TokenRequest(BaseModel):
    text: str = Field(alias="Text")
    model: str = Field(alias="Model")

    class Config:
        populate_by_name = True


@lru_cache(maxsize=len(MODEL_NAME_BY_KEY))
def get_tokenizer(model_key: str):
    try:
        return AutoTokenizer.from_pretrained(MODEL_NAME_BY_KEY[model_key])
    except KeyError as exc:
        raise HTTPException(status_code=404, detail=f"Model '{model_key}' not found.") from exc


@app.post("/tokenize")
def tokenize(req: TokenRequest):
    model_key = req.model.strip().lower()

    tokenizer = get_tokenizer(model_key)

    encoded = tokenizer(req.text, add_special_tokens=False)
    ids = encoded["input_ids"]
    tokens = tokenizer.convert_ids_to_tokens(ids)

    return {"Tokens": tokens, "Ids": ids}
